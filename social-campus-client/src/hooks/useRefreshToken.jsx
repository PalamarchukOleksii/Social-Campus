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

    try {
      const { data } = await axios.post(REFRESH_URL, {
        accessToken: auth.accessToken,
      });

      setAuth((prev) => ({
        ...prev,
        accessToken: data.accessToken,
        accessTokenExpirationInSeconds: data.accessTokenExpirationInSeconds,
      }));

      return data.accessToken;
    } catch (error) {
      console.error("Error refreshing token:", error);
    }
  };

  return refresh;
}

export default useRefreshToken;
