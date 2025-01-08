import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import "./SignUp.css";
import { toast } from "react-toastify";
import axios from "../../utils/consts/AxiosBase";

const REGISTER_URL = "api/users/register";
const ALLOWED_DOMAINS = ["lll.kpi.ua"];

function SignUp() {
  const navigate = useNavigate();

  const [login, setLogin] = useState("");
  const [firstName, setFirstName] = useState("");
  const [secondName, setSecondName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSignUp = async (e) => {
    e.preventDefault();

    const options = {
      method: "POST",
      url: REGISTER_URL,
      headers: { "Content-Type": "application/json" },
      data: JSON.stringify({
        login: login.trim(),
        firstName: firstName.trim(),
        lastName: secondName.trim(),
        email: email.trim(),
        password: password.trim(),
      }),
    };

    try {
      await axios.request(options);

      toast("Your account has been successfully created! Welcome aboard!");
      navigate("/signin");
    } catch (error) {
      if (error.response) {
        if (error.response.data && error.response.data.error) {
          error.response.data.error.forEach((err) => {
            if (err.code === "Email") {
              toast.error(
                `${err.message}. Allowed domains: ${ALLOWED_DOMAINS.join(", ")}`
              );
            } else {
              toast.error(err.message);
            }
          });
        } else if (error.response.data && error.response.data.detail) {
          toast.error(error.response.data.detail);
        } else {
          toast.error("There was an error creating your account.");
        }
      } else {
        console.error(error);
        toast.error("An unexpected error occurred.");
      }
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
              <input
                type="text"
                placeholder="Login"
                value={login}
                onChange={(e) => setLogin(e.target.value)}
                required
              />
              <input
                type="text"
                placeholder="First Name"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                required
              />
              <input
                type="text"
                placeholder="Second Name"
                value={secondName}
                onChange={(e) => setSecondName(e.target.value)}
                required
              />
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
