fetch("https://###WorldServer###0.forgeofempires.com/start/index?action=fetch_worlds_for_login_page", {
    "credentials": "include",
    "headers": {
        "accept": "text/plain, */*; q=0.01",
        "accept-language": "de",
        "cache-control": "no-cache",
        "content-type": "application/x-www-form-urlencoded; charset=UTF-8",
        "pragma": "no-cache",
        "sec-fetch-mode": "cors",
        "sec-fetch-site": "same-origin",
        "x-requested-with": "XMLHttpRequest"
    },
    "referrer": "https://###WorldServer###0.forgeofempires.com/page/",
    "referrerPolicy": "no-referrer-when-downgrade",
    "body": "json=null",
    "method": "POST",
    "mode": "cors"
}).then(res => res.text())
