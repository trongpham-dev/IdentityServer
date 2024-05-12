const config = {
	userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
	authority: "https://localhost:7299/",
	client_id: "client_id_js",
	redirect_uri: "https://localhost:7141/Home/SignIn",
	response_type: "id_token token",
	scope: "openid apione apitwo rc.scope"
}

const userManager = new Oidc.UserManager(config);

const signIn = () => {
	userManager.signinRedirect();
}

userManager.getUser().then(user => {
	console.log(user)
	if (user) {
		axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token
	}
})

const callApi = () => {
	axios.get("https://localhost:7039/secret")
		.then(res => {
			console.log(res)
		})
}

let refreshing = false;

axios.interceptors.response.use((response) => response, (error) => {
	console.log("axios error: ", error.response)

	let axiosConfig = error.response.config

	// if error response 401 try to refresh token
	if (error.response.status === 401) {
		console.log("axios 401")
		// if already refreshing then dont need to make another request
		if (!refreshing) {
			console.log("axios error: ", error)
			refreshing = true

			// do the request
			return userManager.signinSilent().then(user => {
				console.log("new user: ",user)
				//update http client
				axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token

				//update http request
				axiosConfig.headers["Authorization"] = "Bearer " + user.access_token
				//retry the http request
				return axios(axiosConfig)
			})
		}

	}
	return Promise.reject(error)
})
