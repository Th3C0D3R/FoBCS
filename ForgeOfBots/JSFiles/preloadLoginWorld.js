var callback = arguments[arguments.length - 1];
if (document.readyState == "complete") {
	FoELogin()
		.then((v, f, r) => {
			callback(v);
		});
} else {
	document.addEventListener("DOMContentLoaded", () => {
		FoELogin().then((v, f, d) => {
			callback(v);
		});
	});
}

async function FoELogin() {
	await FoETimer(1000);
	if (window.location.href.indexOf("##server##0") <= 0) {
		await FoETimer(1000);
		var x = await fetch("https://##server##.forgeofempires.com/glps/login_check", {
			"credentials": "include",
			"headers": {
				"User-Agent": "##UserAgent##",
				"Accept": "application/json, text/plain, */*",
				"Accept-Language": "de,en-US;q=0.7,en;q=0.3",
				"X-XSRF-TOKEN": "###XSRF-TOKEN###",
				"Content-type": "application/x-www-form-urlencoded; charset=UTF-8",
				"X-Requested-With": "XMLHttpRequest",
				"Pragma": "no-cache",
				"Cache-Control": "no-cache"
			},
			"referrer": "https://##server##.forgeofempires.com/",
			"body": "login%5Buserid%5D=###USERNAME###&login%5Bpassword%5D=###PASSWORD###&login%5Bremember_me%5D=false",
			"method": "POST",
			"mode": "cors"
		});
		window.location = "https://##server##0.forgeofempires.com/page/";
		//return Promise.resolve("redirect");
	} else {
		await FoETimer(1000);
		var res = await fetch("https://##server##0.forgeofempires.com/start/index?action=fetch_worlds_for_login_page", {
			"credentials": "include",
			"headers": {
				"User-Agent": "##UserAgent##",
				"accept": "text/plain, */*; q=0.01",
				"accept-language": "de",
				"cache-control": "no-cache",
				"content-type": "application/x-www-form-urlencoded; charset=UTF-8",
				"pragma": "no-cache",
				"sec-fetch-mode": "cors",
				"sec-fetch-site": "same-origin",
				"x-requested-with": "XMLHttpRequest"
			},
			"referrer": "https://##server##0.forgeofempires.com/page/",
			"referrerPolicy": "no-referrer-when-downgrade",
			"body": "json=null",
			"method": "POST",
			"mode": "cors"
		});
		var body = await res.text();
		if (##t##) {
			console.log(body);
			return Promise.resolve(body); //window.jsInterface.hook(body, "Cities", "ChooseServer", "");
		} else {
			var res2 = await fetch("https://##server##0.forgeofempires.com/start/index?action=play_now_login", {
				"credentials": "include",
				"headers": {
					"User-Agent": "##UserAgent##",
					"Accept": "text/plain, */*; q=0.01",
					"Accept-Language": "de,en-US;q=0.7,en;q=0.3",
					"Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
					"X-Requested-With": "XMLHttpRequest",
					"Pragma": "no-cache",
					"Cache-Control": "no-cache"
				},
				"referrer": "https://##server##0.forgeofempires.com/page/",
				"body": "json=%7B%22world_id%22%3A%22" + ##city## + "%22%7D",
				"method": "POST",
				"mode": "cors"
			});
			var j = await res2.json();
			return Promise.resolve(j['login_url']);
		}
	}
}

function FoETimer(time) {
	if (time == void 0) time = 500;
	return new Promise((res) => {
		setTimeout(() => {
			res();
		}, time);
	});
}