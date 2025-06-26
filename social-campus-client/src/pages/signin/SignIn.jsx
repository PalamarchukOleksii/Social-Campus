import React, { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import "./SignIn.css";
import { toast } from "react-toastify";
import axios from "../../utils/api/AxiosBase";
import useAuth from "../../hooks/useAuth";
import Loading from "../../components/loading/Loading";

const LOGIN_URL = "/api/users/login";

function SignIn() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const { setAuth, persist, setPersist } = useAuth();

  const handleSignIn = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await axios.post(LOGIN_URL, {
        email: email.trim(),
        password: password.trim(),
      });

      const { accessToken } = response.data;

      if (accessToken?.trim()) {
        setAuth(response.data);
        toast("Welcome back! You have successfully signed in.");
        navigate("/home");
      } else {
        toast.error("Failed to retrieve access token.");
      }
    } catch (error) {
      const { response } = error;

      if (response?.data?.error) {
        response.data.error.forEach((err) => toast.error(err.message));
      } else if (response?.data?.detail) {
        toast.error(response.data.detail);
      } else {
        console.error(error);
        toast.error("An unexpected error occurred.");
      }
    } finally {
      setLoading(false);
    }
  };

  const togglePersist = () => {
    setPersist((prev) => !prev);
  };

  useEffect(() => {
    localStorage.setItem("persist", persist);
  }, [persist]);

  return (
    <div className="signin-container">
      <div className="left-half">
        <img src="/android-chrome-512x512.png" alt="Logo" className="logo" />
      </div>
      <div className="right-half">
        <div className="signin">
          <div className="login-container">
            <h1 className="top-text general-text">Sign In</h1>
            {loading ? (
              <div className="loading-wrapper-signin">
                <Loading />
              </div>
            ) : (
              <>
                <form onSubmit={handleSignIn} className="login-form">
                  <input
                    className="text-input"
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                  />
                  <input
                    className="text-input"
                    type={showPassword ? "text" : "password"}
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                  />
                  <div className="show-password-check">
                    <input
                      type="checkbox"
                      id="show-pswd"
                      onChange={() => setShowPassword((prev) => !prev)}
                      checked={showPassword}
                      className="show-pswd-checkbox"
                    />
                    <label htmlFor="show-pswd" className="not-general-text">
                      Show Password
                    </label>
                  </div>
                  <div className="persist-check">
                    <input
                      type="checkbox"
                      id="persist"
                      onChange={togglePersist}
                      checked={persist}
                      className="persist-checkbox"
                    />
                    <label htmlFor="persist" className="not-general-text">
                      Trust This Device
                    </label>
                  </div>
                  <button type="submit" className="signin-button">
                    Sign In
                  </button>
                </form>
                <div className="forgot-password-link">
                  <Link to="/forgot-password" className="not-general-text">
                    Forgot Password?
                  </Link>
                </div>
              </>
            )}
          </div>
          {!loading && (
            <div className="goWelcom">
              <Link to="/" className="not-general-text">
                Go Back to Welcome Page
              </Link>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default SignIn;
