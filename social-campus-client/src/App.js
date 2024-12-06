import React, { useState, useEffect } from "react";
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
import RecommendedProfiles from "./components/recommendedProfiles/RecommendedProfiles";
import { CreateItemContextProvider } from "./context/CreateItemContext";
import CompactSidebar from "./components/compactSidebar/CompactSidebar";
import HorizontalNavbar from "./components/horizontalNavbar/HorizontalNavbar";
import HorizontalRecommendedProfiles from "./components/horizontalRecommendedProfiles/HorizontalRecommendedProfiles";

function App() {
  const location = useLocation();

  const notAuthorizePages = [ROUTES.LANDING, ROUTES.SIGN_IN, ROUTES.SIGN_UP];
  const showSidebar = !notAuthorizePages.includes(location.pathname);
  const showRecommendations = !notAuthorizePages.includes(location.pathname);

  const [isCompactSidebar, setIsCompactSidebar] = useState(
    window.innerWidth <= 1230
  );

  useEffect(() => {
    const handleResize = () => {
      setIsCompactSidebar(window.innerWidth <= 1230);
    };

    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  return (
    <div className="App">
      <CreateItemContextProvider>
        {showSidebar && (
          <div className="sidebar-container">
            {isCompactSidebar ? <CompactSidebar /> : <Sidebar />}
          </div>
        )}
        <div className="main-container">
          <div className="horizontal-recommendations-container">
            <HorizontalRecommendedProfiles />
          </div>
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
          <div className="footer-container">
            <Footer />
          </div>
        </div>
        {showRecommendations && (
          <div className="recommendation-container">
            <RecommendedProfiles />
          </div>
        )}
        <div className="bottom-navbar">
          <HorizontalNavbar />
        </div>
        <ToastContainer progressStyle={{ background: "#3a3a3a" }} />
      </CreateItemContextProvider>
    </div>
  );
}

export default App;
