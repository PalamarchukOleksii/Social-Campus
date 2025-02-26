import React, { useState, useEffect } from "react";
import { Routes, Route, useLocation } from "react-router-dom";
import "./App.css";
import Home from "./pages/home/Home";
import SignUp from "./pages/signup/SignUp";
import Profile from "./pages/profile/Profile";
import Messages from "./pages/messages/Messages";
import SignIn from "./pages/signin/SignIn";
import Sidebar from "./components/sidebar/Sidebar";
import Landing from "./pages/landing/Landing";
import EditProfile from "./pages/editProfile/EditProfile";
import Footer from "./components/footer/Footer";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import ROUTES from "./utils/consts/Routes";
import Followers from "./pages/followers/Followers";
import Following from "./pages/following/Following";
import PublicationDetail from "./pages/publicationDetail/PublicationDetail";
import RecommendedProfiles from "./components/recommendedProfiles/RecommendedProfiles";
import CompactSidebar from "./components/compactSidebar/CompactSidebar";
import HorizontalNavbar from "./components/horizontalNavbar/HorizontalNavbar";
import HorizontalRecommendedProfiles from "./components/horizontalRecommendedProfiles/HorizontalRecommendedProfiles";
import UsersSearch from "./pages/usersSearch/UsersSearch";
import TagsSearch from "./pages/tagsSearch/TagsSearch";
import TagDetail from "./pages/tagDetail/TagDetail";
import PrivateChat from "./pages/privateChat/PrivateChat";
import RequireAuth from "./components/requireAuth/RequireAuth";
import PersistLogin from "./components/persistLogin/PersistLogin";

function App() {
  const location = useLocation();

  const authorizePages = [ROUTES.LANDING, ROUTES.SIGN_IN, ROUTES.SIGN_UP];
  const authorizedHorizontalRecommendations = [
    ROUTES.HOME,
    ROUTES.SEARCH,
    ROUTES.USERS_SEARCH,
    ROUTES.TAGS_SEARCH,
    ROUTES.PROFILE,
    ROUTES.MESSAGES,
  ];

  const showSidebar = !authorizePages.includes(location.pathname);
  const showRecommendations = !authorizePages.includes(location.pathname);
  const showHorizontalRecommendations =
    authorizedHorizontalRecommendations.includes(location.pathname);

  const [isCompactSidebar, setIsCompactSidebar] = useState(
    window.innerWidth <= 1230
  );
  const [isPhone, setIsPhone] = useState(window.innerWidth <= 450);

  const fullContainer = authorizePages.includes(location.pathname)
    ? "full-container"
    : "";

  const centerPage = authorizePages.includes(location.pathname)
    ? "center-container"
    : "page-container";

  useEffect(() => {
    const handleResize = () => {
      setIsCompactSidebar(window.innerWidth <= 1230);
      setIsPhone(window.innerWidth <= 450);
    };

    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  const showFooter =
    (authorizePages.includes(location.pathname) && isPhone) || !isPhone;

  return (
    <div className="App">
        {showSidebar && (
          <div className="sidebar-container">
            {isCompactSidebar ? <CompactSidebar /> : <Sidebar />}
          </div>
        )}
        <div className={`main-container ${fullContainer}`}>
          <div className={centerPage}>
            {showHorizontalRecommendations && (
              <div className="horizontal-recommendations-container">
                <HorizontalRecommendedProfiles />
              </div>
            )}
            <Routes>
              <Route exact path={ROUTES.LANDING} element={<Landing />} />
              <Route path={ROUTES.SIGN_IN} element={<SignIn />} />
              <Route path={ROUTES.SIGN_UP} element={<SignUp />} />

              <Route element={<PersistLogin />}>
                <Route element={<RequireAuth />}>
                  <Route path={ROUTES.HOME} element={<Home />} />
                  <Route path={ROUTES.USERS_SEARCH} element={<UsersSearch />} />
                  <Route path={ROUTES.TAGS_SEARCH} element={<TagsSearch />} />
                  <Route path={ROUTES.PROFILE} element={<Profile />} />
                  <Route path={ROUTES.MESSAGES} element={<Messages />} />
                  <Route path={ROUTES.FOLLOWERS} element={<Followers />} />
                  <Route path={ROUTES.FOLLOWING} element={<Following />} />
                  <Route path={ROUTES.EDIT_PROFILE} element={<EditProfile />} />
                  <Route
                    path={ROUTES.PUBLICATIONDETAILS}
                    element={<PublicationDetail />}
                  />
                  <Route path={ROUTES.TAGDETAILS} element={<TagDetail />} />
                  <Route path={ROUTES.PRIVATECHAT} element={<PrivateChat />} />
                </Route>
              </Route>
            </Routes>
          </div>
          {showFooter && (
            <div className="footer-container">
              <Footer />
            </div>
          )}
        </div>
        {showRecommendations && (
          <div className="recommendation-container">
            <RecommendedProfiles />
          </div>
        )}
        {showSidebar && (
          <div className="bottom-navbar">
            <HorizontalNavbar />
          </div>
        )}
        <ToastContainer />
    </div>
  );
}

export default App;
