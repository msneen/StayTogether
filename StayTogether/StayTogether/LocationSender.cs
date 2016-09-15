using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace StayTogether
{
	public class LocationSender
    {
	    private HubConnection _hubConnection;
	    private IHubProxy _chatHubProxy;

	    public LocationSender ()
		{
		}

	    public async Task UpdateGeoLocationAsync(DateTime expireTime)
	    {
	        do
	        {
                var positionVm = await GetLocationAsync();
                await SendSignalR(positionVm);
                await Task.Delay(20000);

            } while (DateTime.Now<expireTime);

	    }

	    public async Task<PositionVm> GetLocationAsync()
	    {
	        var locator = CrossGeolocator.Current;

	        locator.DesiredAccuracy = 100; //100 is new default


	            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                var positionVm = new PositionVm
                {
                    Position = position,
                    UserName = "mike",
                    PhoneNumber = "619-922-4340"
                };

	        return positionVm;

	    }

	    public async Task InitializeSignalRAsync()
	    {
            // Connect to the server
            _hubConnection = new HubConnection("http://server.com/");

            // Create a proxy to the 'ChatHub' SignalR Hub
            _chatHubProxy = _hubConnection.CreateHubProxy("PositionHub");//I think this string will be the name of Jeff's main class

            // Wire up a handler for the 'UpdateChatMessage' for the server
            // to be called on our client
            //chatHubProxy.On<string>("UpdateChatMessage", message =>
            //    text.Text += string.Format("Received Msg: {0}\r\n", message));

            // Start the connection
            await _hubConnection.Start();

	    }

        public async Task StartGroup()
	    {
            await _chatHubProxy.Invoke("StartGroup", "6199224340");
        }

	    public async Task SendSignalR(PositionVm positionVm)
	    {
            // Invoke the 'UpdatePosition' method on the server
            await _chatHubProxy.Invoke("UpdatePosition", positionVm);

	    }
	}
}

