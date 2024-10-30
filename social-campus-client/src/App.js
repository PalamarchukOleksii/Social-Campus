import React from "react";
import "./App.css";
import { Routes, Route, useLocation } from "react-router-dom";
import Home from "./pages/home/Home";
import SignUp from "./pages/signup/SignUp";
import Search from "./pages/search/Search";
import Profile from "./pages/profile/Profile";
import Messages from "./pages/messages/Messages";
import SignIn from "./pages/signin/SignIn";
import Sidebar from "./components/sidebar/Sidebar";
import Landing from "./pages/landing/Landing";
import Footer from "./components/footer/Footer";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  const location = useLocation();

  const hideSidebarPaths = ["/", "/signin", "/signup"];
  const showSidebar = !hideSidebarPaths.includes(location.pathname);

  let mainContainerClass = "main-container";
  if (["/", "/signin", "/signup"].includes(location.pathname)) {
    mainContainerClass = "main-full-width";
  }

  return (
    <div className="App">
      {showSidebar && <Sidebar />}
      <div className={mainContainerClass}>
        <Routes>
          <Route exact path={"/"} element={<Landing />} />
          <Route path={"/home"} element={<Home />} />
          <Route path={"/signin"} element={<SignIn />} />
          <Route path={"/signup"} element={<SignUp />} />
          <Route path={"/search"} element={<Search />} />
          <Route path={"/profile"} element={<Profile />} />
          <Route path={"/messages"} element={<Messages />} />
        </Routes>
      </div>
      <div className="footer-container">
        <Footer />
      </div>
      <ToastContainer />
    </div>
  );
}

export default App;
