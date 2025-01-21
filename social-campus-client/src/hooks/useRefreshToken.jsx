import axios from "../utils/api/AxiosBase";
import useAuth from "./useAuth";

const REFRESH_URL = "/api/refreshtokens/refresh";

function useRefreshToken() {
  const { setAuth } = useAuth();

  const refresh = async () => {
    try {
      const { data } = await axios.post(REFRESH_URL);

      setAuth((prev) => ({
        ...prev,
        ...data,
      }));

      return data.accessToken;
    } catch (error) {
      console.error("Error refreshing token:", error);
    }
  };

  return refresh;
}

export default useRefreshToken;
