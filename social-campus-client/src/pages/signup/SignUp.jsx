import React from "react";
import { useNavigate, Link } from "react-router-dom";
import "./SignUp.css";
import { toast } from "react-toastify";

function SignUp() {
  const navigate = useNavigate();

  const handleSignUp = async (e) => {
    e.preventDefault();
    try {
      navigate("/home");
      toast("Your account has been successfully created! Welcome aboard!");
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="signup-container">
      <div className="left-half">
        <img src="/android-chrome-512x512.png" alt="Logo" className="logo" />
      </div>
      <div className="right-half">
        <div className="signup">
          <div>
            <h1 className="general-text">Create a New Account</h1>
            <form onSubmit={handleSignUp}>
              <input type="text" placeholder="Login" required />
              <input type="text" placeholder="First Name" required />
              <input type="text" placeholder="Second Name" required />
              <input type="email" placeholder="Email" required />
              <input type="password" placeholder="Password" required />
              <button type="submit" className="signup-button">
                Sign Up
              </button>
            </form>
          </div>
          <div>
            <h2 className="not-general-text">Already have an account?</h2>
            <button
              className="signin-button"
              onClick={() => navigate("/signin")}
            >
              Sign In
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

export default SignUp;
