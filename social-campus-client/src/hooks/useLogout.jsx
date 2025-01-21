import { useEffect } from "react";
import useAuth from "./useAuth";
import useAxiosPrivate from "./useAxiosPrivate";

const REVOKE_URL = "/api/refreshtokens/revoke";

const useLogout = () => {
  const { setAuth, persist, setPersist } = useAuth();
  const axios = useAxiosPrivate();

  const togglePersist = () => {
    setPersist((prev) => !prev);
  };

  useEffect(() => {
    localStorage.setItem("persist", persist);
  }, [persist]);

  const logout = async () => {
    try {
      togglePersist();
      await axios.delete(REVOKE_URL);
    } catch (err) {
      console.error("Error during logout:", err);
    } finally {
      setAuth({});
    }
  };

  return logout;
};

export default useLogout;
