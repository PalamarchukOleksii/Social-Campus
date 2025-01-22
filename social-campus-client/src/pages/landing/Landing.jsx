import React from "react";
import { useNavigate } from "react-router-dom";
import "./Landing.css";

function Landing() {
  const navigate = useNavigate();

  return (
    <div className="landing-container">
      <div className="left-half">
        <img src="/android-chrome-512x512.png" alt="Logo" className="logo" />
      </div>
      <div className="right-half">
        <div className="welcom">
          <div className="top-container">
            <div className="text-container">
              <h1 className="welcome-text general-text">
                Welcome to Social Campus
              </h1>
              <p className="slogan-text not-general-text">
                Connect, Collaborate, and Create
              </p>
            </div>
            <button
              className="register-button"
              onClick={() => navigate("/signup")}
            >
              Create a New Profile
            </button>
          </div>
          <div className="have-account-container">
            <h3 className="have-account-text not-general-text">
              Already have an account?
            </h3>
            <button
              className="login-button"
              onClick={() => navigate("/signin")}
            >
              Sign In
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Landing;
