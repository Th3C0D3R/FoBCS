async function makeRequest() {
    await CefSharp.BindObjectAsync("jsInterface");
    let res = await fetch("https://##WorldID##.forgeofempires.com/game/json?h=##UserKey##", {
        "credentials": "include",
        "headers": {
            "accept": "*/*",
            "accept-language": "en-US,en;q=0.9,de-DE;q=0.8,de;q=0.7",
            "client-identification": "version=##Version##; requiredVersion=##Version##; platform=bro; platformType=html5; platformVersion=web",
            "content-type": "application/json",
            "sec-fetch-mode": "cors",
            "sec-fetch-site": "same-origin",
            "signature": "##sig##",
            "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36"
        },
        "referrer": "https://##WorldID##.forgeofempires.com/game/index?",
        "referrerPolicy": "no-referrer-when-downgrade",
        "body": '##RequestData##',
        "method": "POST",
        "mode": "cors"
    });
    if (res.status === 200) {
        let body = await res.text();
        try {
            var json = JSON.parse(body);
            if (##onlyOne##) {
                for (var i = 0; i < json.length; i++) {
                    const resData = json[i];
                    if (resData["requestMethod"] == "##methode##") {
                        body = JSON.stringify(json[i]);
                        break;
                    }
                }
            }
            else {
                var newBody = "";
                for (var i = 0; i < json.length; i++) {
                    const resData = json[i];
                    if (resData["requestMethod"] == "getData") 
                        newBody += "getData"+JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getMetadata")
                        newBody += "getMetadata" +JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getPlayerResources")
                        newBody += "getPlayerResources" +JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getResourceDefinitions")
                        newBody += "getResourceDefinitions" +JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getOverview")
                        newBody += "getOverview" +JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getOtherTavernStates")
                        newBody += "getOtherTavernStates" +JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getSittingPlayersCount")
                        newBody += "getSittingPlayersCount" + JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getUpdates")
                        newBody += "getUpdates" + JSON.stringify(json[i]) + "##@##";
                    if (resData["requestMethod"] == "getLimitedBonuses")
                        newBody += "getLimitedBonuses" + JSON.stringify(json[i]) + "##@##";
                }
                body = newBody;
            }
            if (json[0]["__class__"] === "Error" || json[0]["__class__"] === "Redirect")
                window.jsInterface.hook("SESSION-EXPIRED", "Data", "##resType##");
            window.jsInterface.hook(body, "Data", "##resType##");
        } catch (error) {
            window.jsInterface.hook("{}", "Data","##resType##");
        }
    }
    else
        window.jsInterface.hook("{}", "Data", "##resType##");
}

makeRequest();