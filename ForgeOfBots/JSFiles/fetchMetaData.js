async function makeRequest() {
    await CefSharp.BindObjectAsync("jsInterface");
    let res = await fetch("##url##", {
        "headers": {
            "accept": "*/*",
            "accept-language": "en-US,en;q=0.9,de-DE;q=0.8,de;q=0.7",
            "sec-fetch-mode": "cors",
            "sec-fetch-site": "same-origin",
            "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36"
        },
        "referrer": "https://##WorldID##.forgeofempires.com/game/index?",
        "referrerPolicy": "no-referrer-when-downgrade",
        "method": "GET",
        "mode": "cors"
    });
    if (res.status === 200) {
        let body = await res.text();
        try {
            var json = JSON.parse(body);
            console.log(body);
            if (json[0]["__class__"] === "Error" || json[0]["__class__"] === "Redirect")
                window.jsInterface.hook("SESSION-EXPIRED", "MetaData", "##resType##");
            window.jsInterface.hook(body, "MetaData", "##resType##");
        } catch (error) {
            window.jsInterface.hook(JSON.parse("[]"), "MetaData", "##resType##");
        }
    }
    else
        window.jsInterface.hook(JSON.parse("[]"), "MetaData", "##resType##");
}
makeRequest();