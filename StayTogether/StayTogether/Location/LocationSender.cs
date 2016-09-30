using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.LocalNotifications;
using Plugin.Settings;
using StayTogether.Classes;

namespace StayTogether
{
	public class LocationSender
    {
	    private HubConnection _hubConnection;
	    private IHubProxy _chatHubProxy;
	    private IGeolocator _geoLocator;
	    private string _phoneNumber;
        private string _groupId = ""; //Creator of group's phone number

        public LocationSender ()
	    {
	        GetNickname();
            GetPhoneNumber();
	    }

	    public void InitializeSignalRAsync()
        {
            // Connect to the server
            //_hubConnection = new HubConnection("http://76.167.114.111/StayTogetherServer/");//jeff
            _hubConnection = new HubConnection("http://162.231.59.41/StayTogetherServer/");//mike

            // Create a proxy to the 'ChatHub' SignalR Hub
            _chatHubProxy = _hubConnection.CreateHubProxy("StayTogetherHub");
            //I think this string will be the name of Jeff's main class

            // Wire up a handler for the 'UpdateChatMessage' for the server
            // to be called on our client
            _chatHubProxy.On<string, string>("BroadcastMessage", ReceiveGroupMessage);
            _chatHubProxy.On<string>("UpdateGroupId", UpdateGroupId);
            _chatHubProxy.On<string, string, string>("SomeoneIsLost", SomeoneIsLost);
            _chatHubProxy.On("GroupDisbanded", GroupDisbanded);

            // Start the connection
            _hubConnection.Start().Wait();

            SetUpLocationEvents();
        }



	    public void SetUpLocationEvents()
	    {

	        _geoLocator = CrossGeolocator.Current;

	        _geoLocator.DesiredAccuracy = 100; //100 is new default

	        if (_geoLocator.IsGeolocationEnabled && _geoLocator.IsGeolocationAvailable)
	        {
	            _geoLocator.PositionChanged += LocatorOnPositionChanged;
                _geoLocator.StartListeningAsync(minTime: 10000, minDistance: 5);
	        }

	    }

	    private void LocatorOnPositionChanged(object sender, PositionEventArgs positionEventArgs)
	    {
           
            var groupMemberVm = new GroupMemberVm()
            {
                Latitude = positionEventArgs.Position.Latitude,
                Longitude = positionEventArgs.Position.Longitude,
                PhoneNumber = _phoneNumber
            };

            SendUpdatePosition(groupMemberVm);
        }



	    private void GroupDisbanded()
	    {
	        _groupId = "";
            AddNotification("Group Disbanded", "Your Group has been disbanded");
        }

	    public void UpdateGroupId(string id)
	    {
	        _groupId = id;
	    }

	    public void SomeoneIsLost(string phoneNumber, string latitude, string longitude)
	    {
	        if (!string.IsNullOrWhiteSpace(_groupId))
	        {
	            AddNotification("YOU LOST SOMEONE", $"{phoneNumber} {latitude}   {longitude}");
	        }
	    }

	    public void ReceiveGroupMessage(string phoneNumber, string message)
	    {
	        AddNotification("Stay Together Update", message);
	    }


	    private void AddNotification(string title, string message)
	    {
            CrossLocalNotifications.Current.Show(title, message);            
	    }

	    public async Task StartGroup(GroupVm groupVm)
	    {
            await _chatHubProxy.Invoke("CreateGroup", groupVm);
        }

	    public void SendUpdatePosition(GroupMemberVm groupMemberVm)
	    {
	        groupMemberVm.PhoneNumber = _phoneNumber;
	        groupMemberVm.GroupId = _groupId;
            _chatHubProxy.Invoke("updatePosition", groupMemberVm);
        }


        private void GetNickname()
        {
            var nickName = CrossSettings.Current.GetValueOrDefault<string>("nickname");
            if (string.IsNullOrWhiteSpace(nickName))
            {
                AddNotification("StayTogether nickname", "Please Add your nickname in settings");
            }
        }

        private void GetPhoneNumber()
        {
            _phoneNumber = CrossSettings.Current.GetValueOrDefault<string>("phonenumber");
            if (string.IsNullOrWhiteSpace(_phoneNumber))
            {
                AddNotification("StayTogether PhoneNumber", "Please Add your Phone Number in settings");
            }
        }

        public Task SendError(string message)
	    {
	        _chatHubProxy.Invoke("SendErrorMessage", message, _phoneNumber);
            return Task.CompletedTask;
	    }
    }
}

