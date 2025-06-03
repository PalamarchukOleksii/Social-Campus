import axios from "../utils/api/AxiosBase";
import { useEffect } from "react";
import useRefreshToken from "./useRefreshToken";
import useAuth from "./useAuth";
import { useLocation, useNavigate } from "react-router-dom";
import ROUTES from "../utils/consts/Routes";
import { toast } from "react-toastify";

function useAxiosPrivate() {
  const refresh = useRefreshToken();
  const { auth, setAuth } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const requestIntercept = axios.interceptors.request.use(
      (config) => {
        if (!config.headers["Authorization"] && auth?.accessToken) {
          config.headers["Authorization"] = `Bearer ${auth.accessToken}`;
        }
        return config;
      },
      (error) => Promise.reject(error)
    );

    const responseIntercept = axios.interceptors.response.use(
      (response) => response,
      async (error) => {
        const prevRequest = error?.config;
        if (error?.response) {
          const status = error.response.status;
          if (status === 401 && !prevRequest?.sent) {
            prevRequest.sent = true;
            try {
              const newAccessToken = await refresh();
              if (newAccessToken) {
                prevRequest.headers[
                  "Authorization"
                ] = `Bearer ${newAccessToken}`;
                return axios(prevRequest);
              }
            } catch (refreshError) {
              if (refreshError.response?.status === 400) {
                setAuth(null);
                navigate(ROUTES.SIGN_IN, {
                  state: { from: location },
                  replace: true,
                });
                toast.error("Session expired. Please sign in again.");
              } else {
                toast.error(
                  refreshError.response?.data?.message ||
                    "Failed to refresh token."
                );
              }
              return Promise.reject(refreshError);
            }
          }

          switch (status) {
            case 400:
              toast.error(
                error.response.data?.message ||
                  "Bad request. Please check your input."
              );
              break;
            case 403:
              toast.error(
                error.response.data?.message ||
                  "Access forbidden. You do not have permission."
              );
              break;
            case 404:
              toast.error(
                error.response.data?.message || "Resource not found."
              );
              break;
            case 500:
              toast.error(
                error.response.data?.message ||
                  "Server error. Please try again later."
              );
              break;
            default:
              toast.error(
                error.response.data?.message ||
                  `Unexpected error occurred (Status: ${status}).`
              );
              break;
          }
        } else {
          toast.error(error.message || "Network error. Please try again.");
        }
        return Promise.reject(error);
      }
    );

    return () => {
      axios.interceptors.request.eject(requestIntercept);
      axios.interceptors.response.eject(responseIntercept);
    };
  }, [auth, refresh, setAuth, navigate, location]);

  return axios;
}

export default useAxiosPrivate;
