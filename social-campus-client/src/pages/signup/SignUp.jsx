// SignUp.js
import React from "react";
import { useNavigate } from "react-router-dom";
import "./SignUp.css";

function SignUp() {
  const navigate = useNavigate();

  const handleSignUp = async (e) => {
    e.preventDefault();
    try {
      // Handle sign-up logic here
      console.log("Sign up logic goes here");
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="signup">
      <div className="signup-option">
        <h1 className="general-text">Create a New Account</h1>
        <form onSubmit={handleSignUp}>
          <input type="text" placeholder="First Name" required />
          <input type="text" placeholder="Second Name" required />
          <input type="email" placeholder="Email" required />
          <input type="password" placeholder="Password" required />
          <button type="submit" className="signup-button">
            Sign Up
          </button>
        </form>
        <h2 className="not-general-text">Already have an account?</h2>
        <button className="signin-button" onClick={() => navigate("/signin")}>
          Sign In
        </button>
      </div>
    </div>
  );
}

export default SignUp;
