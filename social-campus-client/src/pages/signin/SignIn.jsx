import React from "react";
import { useNavigate } from "react-router-dom";
import "./SignIn.css";
import { toast } from "react-toastify";

function SignIn() {
  const navigate = useNavigate();

  const handleSignIn = async (e) => {
    e.preventDefault();
    try {
      // Handle sign-in logic here
      console.log("Sign in normally");
      navigate("/home");
      toast("Welcome back! You have successfully signed in.");
    } catch (error) {
      console.log(error);
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
              <input type="email" placeholder="Email" required />
              <input type="password" placeholder="Password" required />
              <button type="submit" className="signin-button">
                Sign In
              </button>
            </form>
          </div>
          <div>
            {" "}
            <h2 className="not-general-text">Don&apos;t have an account?</h2>
            <button
              className="signup-button"
              onClick={() => navigate("/signup")}
            >
              Sign Up
            </button>
          </div>
          <div className="goWelcom">
            <a href="/" className="not-general-text">
              Go Back to Welcome Page
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}

export default SignIn;
