using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.AspNet.SignalR.Client;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using StayTogether.Droid;

namespace StayTogether
{
	public class LocationSender
    {
	    private HubConnection _hubConnection;
	    private IHubProxy _chatHubProxy;
	    private static Guid GroupId = Guid.Empty;
	    private IGeolocator _geoLocator;
	    private string _phoneNumber;
	    private NotificationManager _notificationManager;
	    private Context _context;

	    public LocationSender (Context context, NotificationManager notificationManager, string phoneNumber)
	    {
	        _context = context;
	        _notificationManager = notificationManager;
	        _phoneNumber = phoneNumber;
	    }


	    public void SetUpLocationEvents()
	    {
	        try
	        {
	            _geoLocator = CrossGeolocator.Current;

	            _geoLocator.DesiredAccuracy = 100; //100 is new default

	            if (_geoLocator.IsGeolocationEnabled && _geoLocator.IsGeolocationAvailable)
	            {
	                _geoLocator.PositionChanged += LocatorOnPositionChanged;
                    _geoLocator.StartListeningAsync(minTime: 10000, minDistance: 5);

	            }

                

	        }
	        catch (Exception ex)
	        {
	            
	        }
	    }

	    private void LocatorOnPositionChanged(object sender, PositionEventArgs positionEventArgs)
	    {
           
            var positionVm = new PositionVm
            {
                Position = positionEventArgs.Position,
                PhoneNumber = _phoneNumber
            };

            SendSignalR(positionVm);
        }

	    public void InitializeSignalRAsync()
	    {
	        try
	        {
	            // Connect to the server
	            _hubConnection = new HubConnection("http://162.231.59.41/StayTogetherServer/");

	            // Create a proxy to the 'ChatHub' SignalR Hub
	            _chatHubProxy = _hubConnection.CreateHubProxy("StayTogetherHub");
	            //I think this string will be the name of Jeff's main class

	            // Wire up a handler for the 'UpdateChatMessage' for the server
	            // to be called on our client
	            _chatHubProxy.On<string,string>("BroadcastMessage", ReceiveGroupMessage);
                _chatHubProxy.On<string, string, string>("SomeoneIsLost", SomeoneIsLost); 

                // Start the connection
                _hubConnection.Start().Wait();

	            SetUpLocationEvents();

	        }
	        catch (Exception ex)
	        {
	            throw ex;
	        }
	    }

	    public void SomeoneIsLost(string phoneNumber, string latitude, string longitude)
	    {
            AddNotification("YOU LOST SOMEONE", $"{phoneNumber} {latitude}   {longitude}");
        }

	    public void ReceiveGroupMessage(string phoneNumber, string message)
	    {
	        AddNotification("Stay Together Update", message);
	    }

	    private void AddNotification(string title, string message)
	    {
            // Instantiate the builder and set notification elements:
	        Notification.Builder builder = new Notification.Builder(_context)
	            .SetContentTitle(title)
	            .SetContentText(message + DateTime.Now.ToShortTimeString())
	            .SetSmallIcon(Resource.Drawable.Icon)
                .SetAutoCancel(true);

	        // Build the notification:
	        Notification notification = builder.Build();

	        // Publish the notification:
	        const int notificationId = 0;
	        _notificationManager.Notify(notificationId, notification);
	    }

	    public async Task StartGroup(UserVm userVm)
	    {
            await _chatHubProxy.Invoke("StartGroup", userVm);
        }

	    public void SendSignalR(PositionVm positionVm)
	    {
            //_chatHubProxy.Invoke("send", _phoneNumber, $"Hi There {positionVm.Position.Latitude}   {positionVm.Position.Longitude}" );
            //UpdatePosition
            _chatHubProxy.Invoke("updatePosition", _phoneNumber, positionVm.Position.Latitude, positionVm.Position.Longitude);
        }

        public void SendSignalR()
        {

            _chatHubProxy.Invoke("send", _phoneNumber, $"Hi There.  I don't know where I am yet.  But I'm working on it");

        }
    }
}

