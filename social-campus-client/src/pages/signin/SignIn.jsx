import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import "./SignIn.css";
import { toast } from "react-toastify";
import axios from "../../utils/consts/AxiosBase";
import useAuth from "../../hooks/useAuth";

const LOGIN_URL = "/api/users/login";

function SignIn() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const { setAuth } = useAuth();

  const handleSignIn = async (e) => {
    e.preventDefault();

    const options = {
      method: "POST",
      url: LOGIN_URL,
      headers: { "Content-Type": "application/json" },
      data: JSON.stringify({
        email: email.trim(),
        password: password.trim(),
      }),
    };

    try {
      const response = await axios.request(options);

      if (
        response.data.accessToken &&
        response.data.accessToken.trim() !== ""
      ) {
        setAuth(response.data);

        toast("Welcome back! You have successfully signed in.");
        navigate("/home");
      } else {
        toast.error("Failed to retrieve access token.");
      }
    } catch (error) {
      if (error.response) {
        if (error.response.data && error.response.data.error) {
          error.response.data.error.forEach((err) => {
            toast.error(err.message);
          });
        } else if (error.response.data && error.response.data.detail) {
          toast.error(error.response.data.detail);
        } else {
          toast.error("There was an error logging into your account.");
        }
      } else {
        console.error(error);
        toast.error("An unexpected error occurred.");
      }
    }
  };

  return (
    <div className="signin-container">
      <div className="left-half">
        <img src="/android-chrome-512x512.png" alt="Logo" className="logo" />
      </div>
      <div className="right-half">
        <div className="signin">
          <div>
            <h1 className="general-text">Sign In</h1>
            <form onSubmit={handleSignIn}>
              <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
              <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <button type="submit" className="signin-button">
                Sign In
              </button>
            </form>
          </div>
          <div>
            <h2 className="not-general-text">Don&apos;t have an account?</h2>
            <button
              className="signup-button"
              onClick={() => navigate("/signup")}
            >
              Sign Up
            </button>
          </div>
          <div className="goWelcom">
            <Link to="/" className="not-general-text">
              Go Back to Welcome Page
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}

export default SignIn;
