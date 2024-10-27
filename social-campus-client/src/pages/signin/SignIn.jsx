// SignIn.js
import React from "react";
import { useNavigate } from "react-router-dom";
import "./SignIn.css";

function SignIn() {
  const navigate = useNavigate();

  const handleSignIn = async (e) => {
    e.preventDefault();
    try {
      // Handle sign-in logic here
      console.log("Sign in logic goes here");
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="signin">
      <h1 className="general-text">Sign In</h1>
      <form onSubmit={handleSignIn}>
        <input type="text" placeholder="Email" required />
        <input type="password" placeholder="Password" required />
        <button type="submit" className="signin-button">
          Sign In
        </button>
      </form>
      <h2 className="not-general-text">Don&apos;t have an account?</h2>
      <button className="signup-button" onClick={() => navigate("/signup")}>
        Sign Up
      </button>
    </div>
  );
}

export default SignIn;
