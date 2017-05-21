var myBrowser = null;
var localplayer = "player";

API.onResourceStart.connect(function (sender) {
    var player = API.getLocalPlayer();
    localplayer = API.getPlayerName(player);
    API.sendChatMessage("Your username is " + localplayer);

    var res = API.getScreenResolution(); //this gets the client's screen resoulution
    myBrowser = API.createCefBrowser(res.Width / 2, res.Height / 2); //we're initializing the browser here. This will be the full size of the user's screen.
    API.waitUntilCefBrowserInit(myBrowser); //this stops the script from getting ahead of itself, it essentially pauses until the browser is initialized
    API.setCefBrowserPosition(myBrowser, 0, 0); //The orientation (top left) corner in relation to the user's screen.  This is useful if you do not want a full page browser.  0,0 is will lock the top left corner of the browser to the top left of the screen.
    API.loadPageCefBrowser(myBrowser, "Login/index.html"); //This loads the HTML file of your choice.      .    API.setCefBrowserHeadless(myBrowser, true); //this will remove the scroll bars from the bottom/right side
    API.showCursor(true); //This will show the mouse cursor
    API.setCanOpenChat(false);  //This disables the chat, so the user can type in a form without opening the chat and causing issues.
});

API.onResourceStop.connect(function (e, ev) {
    if (myBrowser != null) {
        API.destroyCefBrowser(myBrowser);
    }
});

API.onServerEventTrigger.connect(function (name, args) {
    if (name == "showLogin") {
        API.setCefBrowserHeadless(myBrowser, false);
       // API.showCursor(true);
    }
});

function login(Password) {
    API.sendChatMessage("Your password is " + Password); //send a chat message with the data they entered.
    API.showCursor(false); //stop showing the cursor
    API.destroyCefBrowser(myBrowser); //destroy the CEF browser
    API.setCanOpenChat(true); //allow the player to use the chat again.

    API.triggerServerEvent("PlayerLogin",Password);
}

function getUser() {
    var player = API.getLocalPlayer();
    return API.getPlayerName(player);
}
