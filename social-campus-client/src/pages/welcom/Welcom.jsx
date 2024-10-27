import React from "react";
import { useNavigate } from "react-router-dom";
import "./Welcom.css";

import { toast } from "react-toastify";

function Welcom() {
  const navigate = useNavigate();

  const handleGoogle = async (e) => {
    e.preventDefault();
    try {
      // Handle sign-in with Google
      console.log("Sign up with Google");
      navigate("/home");
      toast("Your account has been successfully created! Welcome aboard!");
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="welcom">
      <h1 className="general-text">Welcome to Social Campus</h1>
      <p className="not-general-text">Connect, Collaborate, and Create</p>
      <div className="button-container">
        <button className="google-button" onClick={handleGoogle}>
          Sign Up with Google
        </button>
        <button className="profile-button" onClick={() => navigate("/signin")}>
          Create a New Profile
        </button>
      </div>
      <h2 className="not-general-text">Already hava an account?</h2>
      <button className="signup-button" onClick={() => navigate("/signup")}>
        Sign In
      </button>
    </div>
  );
}

export default Welcom;
