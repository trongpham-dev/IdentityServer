
const createSession = () => {
	return "SessionValueMakeItvIASDFSDAFSDFDSFSDFDSFasdasdsadasdasdsadasdasdasdasd"
}

createNonce = () => {
	return "Nonceasdasdasdasdasdasdasdasdasasdasdd"
}

const signIn = () => {
	var redirectUri = "https://localhost:7141/Home/SignIn"
	var responseType = "id_token token"
	var scope = "openid apione"
	var authUrl = "/connect/authorize/callback"+
"?client_id=client_id_js"+
"&redirect_uri=" + encodeURIComponent(redirectUri) +
"&response_type=" + encodeURIComponent(responseType)+
"&scope=" + encodeURIComponent(scope)+
"&nonce=" + createNonce()+
"&state="+ createSession()
	var returnUrl = encodeURIComponent(authUrl)
	console.log(returnUrl)

	window.location.href = "https://localhost:7299/Auth/Login?ReturnUrl=" + returnUrl;
}