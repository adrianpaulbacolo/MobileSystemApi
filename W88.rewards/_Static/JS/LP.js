$(function () { $("a").click(function () { if ($(this).attr('data-load-ignore-splash') != 'true' && $(this).attr('data-rel') != "dialog") { if ($(this).className != null) { if ($(this).className.indexOf('ui-collapsible-heading-toggle') < 0) { GPINTMOBILE.ShowSplash(); } } } }); });

$(window).load(function () { GPINTMOBILE.HideSplash(); });
var LPVariables = new Object();
function Dictionary() {
    var dictionary = {};
    this.setData = function (key, val) { dictionary[key] = val; }
    this.getData = function (key) { return dictionary[key]; }
}

var dictionary = new Dictionary();

(function (window) {//Closure scope for protection of our chat
    var chatArea, chatInput, chatState, chat, logsLastChild;
    var logsStarted = false;
    var tabIndex = 7;

    //Bind method to DOM support IE8 +
    function bindEvent(element, eventName, callback) {
        if (element.addEventListener) {
            element.addEventListener(eventName, callback, false);
        } else {
            element.attachEvent("on" + eventName, callback);
        }
    }
    //Unbinds  method from DOM
    function unBindEvent(element, eventName, callback) {
        if (element.addEventListener) {
            element.removeEventListener(eventName, callback, false);
        } else {
            element.detachEvent("on" + eventName, callback);
        }
    }

    //Get a cleaned input value from the DOM by id
    function getTrimmedValue(inputId, clearValue){
        var res = "";
        var element = document.getElementById(inputId);
        if(element && element.value){
            res = element.value;
            res = res.replace(/^[\s*\n*\r*\t\*]*$[\s*\n*\r*\t\*]*/g, "");
            if(clearValue){
                element.value = '';
            }
        }
        return res;
    }

    //shows an element by it's id
    function showElementById(inputId){
        var elem = document.getElementById(inputId);
        if(elem){
            elem.style.display = 'block';
        }
    }

    //hides an element by it's id
    function hideElementById(inputId){
        var elem = document.getElementById(inputId);
        if(elem){
            elem.style.display = 'none';
        }
    }

    //Add lines to the chat from events
    function addLines(data) {
        var linesAdded = false;
        for (var i = 0; i < data.lines.length; i++) {
            var line =  data.lines[i];
            if(line.source !== 'visitor' || chatState != chat.chatStates.CHATTING) {
                var chatLine = createLine(line);
                addLineToDom(chatLine);
                linesAdded = true;
            }
        }
        if(linesAdded){
            scrollToBottom();
        }
    }
    //Create a chat line
    function createLine(line) {
        var div = document.createElement("DIV");
        if (line.source == "system") {
            switch (line.systemMessageId.toString()) {
                case "3":
                    if (document.getElementById('divChatWith')) { }
                    else {
                        div.innerHTML += dictionary.getData("lblSysMsg3");
                        div.setAttribute("id", "divChatWith");
                    }
                    break;

                case "4":
                    if (document.getElementById('divWaitFor')) { }
                    else {
                        div.innerHTML += dictionary.getData("lblSysMsg4");
                        div.setAttribute("id", "divWaitFor");
                    }
                    break;

                case "5":
                    if (document.getElementById('SysMsg5')) { }
                    else {
                        div.innerHTML += dictionary.getData("lblSysMsg5");
                        div.setAttribute("id", "SysMsg5");
                    }
                    break;

                case "14":
                    if (document.getElementById('SysMsg14')) { }
                    else {
                        div.innerHTML += dictionary.getData("lblSysMsg14");
                        div.setAttribute("id", "SysMsg14");
                    }
                    break;
            }
        }
        else {
            div.innerHTML = line.by + ": ";
            if(line.source === 'visitor' ){
                div.appendChild(document.createTextNode(line.text));
            }else{
                div.innerHTML += line.text;
            } 
        }

        div.setAttribute("title", line.by + " " + getText(line.text));
        switch (line.source)
        {
            case "visitor": case "agent":
                div.setAttribute("class", 'chat-bubble ' + line.source);
                break;

            default:
                div.setAttribute("class", line.source);
                break;
        }
        
        div.setAttribute("tabIndex", tabIndex);
        tabIndex = tabIndex + 1;
        return div;
    }

    function getText(text){
        var div = document.createElement("DIV");
        div.appendChild(document.createTextNode(text));
        return div.innerText || div.textContent;
    }

    //Add a line to the chat view DOM
    function addLineToDom(line){
        if (!chatArea) {
            chatArea = document.getElementById("chatLines");
        }
        chatArea.appendChild(line);
    }

    //Scroll to the bottom of the chat view
    function scrollToBottom(){
        if (!chatArea) {
            chatArea = document.getElementById("chatLines");
        }
        chatArea.scrollTop = chatArea.scrollHeight;
    }

    //Sends a chat line
    function sendLine() {
        var text = getTrimmedValue("txtSendMsg", true);
        if (text && chat) {
            var line = createLine({
                by : chat.getVisitorName(),
                text: text,
                source : 'visitor'
            });

            chat.addLine({
                text: text,                
                error: function(){
                    line.className = "error";
                }
            });
            addLineToDom(line);
            scrollToBottom();
        }
    }

    //Listener for enter events in the text area
    function keyChanges(e) {
        e = e || window.event;
        var key = e.keyCode || e.which;
        if (key == 13) {
            if (e.type == "keyup") {
                sendLine();
                setVisitorTyping(false);
            }
            return false;
        }else{
            setVisitorTyping(true);
        }
    }

    //Set the visitor typing state
    function setVisitorTyping(typing){
        if(chat){
            chat.setVisitorTyping({ typing: typing });
        }
    }

    function setVisitorName() {
        if (chat) {
            if (LPVariables.VisitorName != '') {
                var failedRequest = chat.setVisitorName({
                    visitorName: LPVariables.VisitorName,
                    success: chat.nameUpdated,
                    error: chat.nameUpdateFailed,
                    context: chat
                });
                if (failedRequest && failedRequest.error) {
                    //console.log(failedRequest.error);
                }
            }
        }
    }

    //Ends the chat
    function endChat() {
        if (chat) {
            chat.endChat({success: function(){
                $("#btnEndChat > span > span").html(dictionary.getData("lblChatEnded"));
            }});
        }
    }

    //Sets a custom variable in the chat session
    function setCustomVariable() {
        var val = getTrimmedValue("varValue", true);
        var name = getTrimmedValue("varName", true);
        if (val && name && chat) {
            var vars = {};
            vars[name] = val;
            chat.setCustomVariable({
                customVariables: vars,
                error : function(){

                }
            });
        }
    }

    //Sends an email of the transcript when the chat has ended
    function sendEmail(){
        var email = getTrimmedValue("emailAddress", true);
        if(chat && email){
            chat.requestTranscript({email: email});
        }
    }

    //Sets the local chat state
    function updateChatState(data){
        chatState = data.state;
        switch (chatState) {
            case "waiting":
                //hideElementById("btnReqChat");
                hideChatRequest();
                break;

            case "chatting":
                hideElementById("divWaitFor");
                showElementById("divChatSend");
                showElementById("btnEndChat");
                if (document.getElementById("divChatWith").innerHTML.indexOf('{LPOPERATOR}') >= 0) {
                    setAgentName(data);
                }
                break;

            case "resume":
                hideElementById("divWaitFor");
                break;
            case "ended":
                if (document.getElementById("divChatWith").innerHTML.indexOf('{LPOPERATOR}') >= 0) { setAgentName(data); }
                hideElementById("divWaitFor");
                $("#btnEndChat > span > span").html(dictionary.getData("lblChatEnded"));
                break;
        }
    }

    function agentTyping(data){
        if (data.agentTyping) { showElementById("agentIsTyping"); }
        else { hideElementById("agentIsTyping"); }
    }

    function setAgentName(data) {
        var strAgentName = $('#divChatWith').attr('title').substring($('#divChatWith').attr('title').indexOf("'"));
        if (document.getElementById("divChatWith").innerHTML.indexOf('{LPOPERATOR}') >= 0) {
            if (data.agentName != '') { document.getElementById("divChatWith").innerHTML = document.getElementById("divChatWith").innerHTML.replace('{LPOPERATOR}', strAgentName); }
        }
    }

    //starts a chat
    function startChat() {
        hideChatRequest();
        showElementById("divWaitFor");
        startChatApi();

        var strArray = ['Hello I need support', 'Can you help me?'];

        var failedRequest = chat.requestChat({
            skill: LPVariables.Skill,
            customVariables: {
                customVariable: [
                { name: "memcode", value: LPVariables.VisitorName },
                { name: "source", value: "mobile" },
                { name: "domain", value: document.domain }
                ]
            },
            preChatLines: strArray,
            referrer: "https://liveperson.com"
            });

    }
    //Shows the chat request button
    function showChatRequest(){
        showElementById("btnReqChat");
    }
    //hides the chat request button
    function hideChatRequest(){
        hideElementById("btnReqChat");
        var div = document.createElement("DIV");
        if (document.getElementById('divWaitFor')) { }
        else {
            div.innerHTML += dictionary.getData("lblSysMsg4");
            div.setAttribute("id", "divWaitFor");
        }
        addLineToDom(div);
    }

    function showSendRequest() { showElementById("divChatSend"); }
    function hideSendRequest() { showElementById("divChatSend"); }
    //Writes a log line for events
    //So we can debug some non visual stuff
    function writeLog(logName, data) {
        data = typeof data === 'string' ? data : JSON.stringify(data);
        var date = new Date();
        date = "" + (date.getHours() > 10 ? date.getHours() : "0" + date.getHours()) +
            ":" + (date.getMinutes() > 10 ? date.getMinutes() : "0" + date.getMinutes()) +
            ":" + (date.getSeconds() > 10 ? date.getSeconds() : "0" + date.getSeconds());

        //console.log(date + " " + logName + " : " + data);
    }

    //Creates an instance of the chat API with method binding
    function startChatApi(){
        var lpNumber = LPVariables.LPNumber; 
        var appKey = LPVariables.AppKey;
        if(lpNumber && appKey){
            chat = new lpTag.taglets.ChatOverRestAPI({
                lpNumber: lpNumber,
                appKey: appKey,
                //onInit: [showChatRequest, function (data) { writeLog("onInit", data);} ],
                onInfo: [setAgentName, function (data) { writeLog("onInfo", data); }],
                onLine: [addLines, function (data) { writeLog("onLine", data); }],
                onState: [ updateChatState, function(data){ writeLog("onState", data);} ],
                onStart: [hideChatRequest, updateChatState, bindInputForChat, setVisitorName, function (data) { writeLog("onStart", data); }],
                onStop: [ hideSendRequest, updateChatState, unBindInputForChat],
                onAgentTyping: [agentTyping, function (data) { writeLog("onAgentTyping", data); }],
                //onVisitorName: [setVisitorName, function (data) { writeLog("onVisitorName", data); }],
                onRequestChat: function(data) { writeLog("onRequestChat", data); }
            });
            showElementById("divChatContainer");
        }
    };

    function bindInputForChat(){
        bindEvent(document.getElementById("btnSend"), "click", sendLine);
        bindEvent(document.getElementById("txtSendMsg"), "keyup", keyChanges);
        bindEvent(document.getElementById("txtSendMsg"), "keydown", keyChanges);
        bindEvent(document.getElementById("btnEndChat"), "click", endChat);
    }

    function unBindInputForChat(){
        unBindEvent(document.getElementById("btnSend"), "click", sendLine);
        unBindEvent(document.getElementById("txtSendMsg"), "keyup", keyChanges);
        unBindEvent(document.getElementById("txtSendMsg"), "keydown", keyChanges);
        unBindEvent(document.getElementById("btnEndChat"), "click", endChat);
        unBindEvent(document.getElementById("btnReqChat"), "click", startChat);
        hideElementById("btnReqChat");
        hideElementById("btnEndChat");        
        hideElementById("divChatSend");
        window.lpTag.utils.sessionDataManager.clearSessionData();
        window.location.replace('/Index');
    }

    bindEvent(window, "load", function () {
        bindEvent(document.getElementById("btnReqChat"), "click", startChat);
    });
})(window);