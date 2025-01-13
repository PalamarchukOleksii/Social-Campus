import axios from "../utils/api/AxiosBase";
import useAuth from "./useAuth";

const REFRESH_URL = "/api/refreshtokens/refresh";

function useRefreshToken() {
  const { auth, setAuth } = useAuth();

  const refresh = async () => {
    if (!auth?.accessToken) {
      console.error("No access token available to refresh.");
      return;
    }

    const options = {
      method: "POST",
      url: REFRESH_URL,
      headers: { "Content-Type": "application/json" },
      data: { accessToken: auth.accessToken },
      withCredentials: true,
    };

    try {
      const { data } = await axios.request(options);
      setAuth((prev) => ({
        ...prev,
        accessToken: data.accessToken,
        accessTokenExpirationInMinutes: data.accessTokenExpirationInMinutes,
      }));

      return data.accessToken;
    } catch (error) {
      console.error("Error refreshing token:", error);
    }
  };

  return refresh;
}

export default useRefreshToken;
