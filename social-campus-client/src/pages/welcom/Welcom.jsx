import React from "react";
import { useNavigate } from "react-router-dom";
import "./Welcom.css";

function Welcom() {
  const navigate = useNavigate();

  const handleGoogle = async (e) => {
    e.preventDefault();
    try {
      // Handle sign-in with Google
      console.log("Sign in with Google");
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="welcom">
      <div className="sign-option">
        <h1 className="general-text">Welcome to Social Campus</h1>
        <p className="not-general-text">Connect, Collaborate, and Create</p>
        <div className="button-container">
          <button className="google-button" onClick={handleGoogle}>
            Sign In with Google
          </button>
          <button
            className="profile-button"
            onClick={() => navigate("/signin")}
          >
            Create a New Profile
          </button>
        </div>
        <h2 className="not-general-text">Already heva an account?</h2>
        <button className="signup-button" onClick={() => navigate("/signup")}>
          Sign Up
        </button>
      </div>
    </div>
  );
}

export default Welcom;
