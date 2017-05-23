
API.onKeyDown.connect(function (sender, args)
{

    if (args.KeyCode == Keys.J) {
        API.triggerServerEvent("onKeyDown", 3);
    }

});