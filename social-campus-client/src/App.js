import React from "react";
import { Routes, Route, useLocation } from "react-router-dom";
import "./App.css";
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
import ROUTES from "./utils/consts/Routes";
import Followers from "./pages/followers/Followers";
import Following from "./pages/following/Following";
import PublicationDetail from "./pages/publicationDetail/PublicationDetail";

function App() {
  const location = useLocation();

  const notAuthorizePages = [ROUTES.LANDING, ROUTES.SIGN_IN, ROUTES.SIGN_UP];
  const showSidebar = !notAuthorizePages.includes(location.pathname);
  const mainContainerClass = notAuthorizePages.includes(location.pathname)
    ? "main-full-width"
    : "main-container";
  const footerContainerClass = notAuthorizePages.includes(location.pathname)
    ? "footer"
    : "footer-main";

  return (
    <div className="App">
      {showSidebar && <Sidebar />}
      <div className={mainContainerClass}>
        <Routes>
          <Route exact path={ROUTES.LANDING} element={<Landing />} />
          <Route path={ROUTES.HOME} element={<Home />} />
          <Route path={ROUTES.SIGN_IN} element={<SignIn />} />
          <Route path={ROUTES.SIGN_UP} element={<SignUp />} />
          <Route path={ROUTES.SEARCH} element={<Search />} />
          <Route path={ROUTES.PROFILE} element={<Profile />} />
          <Route path={ROUTES.MESSAGES} element={<Messages />} />
          <Route path={ROUTES.FOLLOWERS} element={<Followers />} />
          <Route path={ROUTES.FOLLOWING} element={<Following />} />
          <Route
            path={ROUTES.PUBLICATIONDETAILS}
            element={<PublicationDetail />}
          />
        </Routes>
      </div>
      <div className={footerContainerClass}>
        <Footer />
      </div>
      <ToastContainer progressStyle={{ background: "#3a3a3a" }} />
    </div>
  );
}

export default App;
