import React, { useState, useEffect } from "react";
import { createPortal } from "react-dom";
import "./Sidebar.css";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import GetSidebarItems from "../../utils/consts/GetSidebarItems";
import ShortProfile from "../shortProfile/ShortProfile";
import useAuth from "../../hooks/useAuth";
import useLogout from "../../hooks/useLogout";
import ROUTES from "../../utils/consts/Routes";
import CreatePublication from "../createPublication/CreatePublication";

function Sidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");
  const [user, setUser] = useState({});
  const [sidebarItems, setSidebarItems] = useState([]);
  const [isCreatePublicationOpen, setIsCreatePublicationOpen] = useState(false);

  const { auth } = useAuth();
  const logout = useLogout();

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
    setUser(currentUser);
    setSidebarItems(GetSidebarItems(currentUser.login || ""));
  }, [auth]);

  const handleLogout = async (e) => {
    e.preventDefault();
    try {
      await logout();
      navigate(ROUTES.LANDING);
    } catch (error) {
      console.error(error);
    }
  };

  const handleOpenCreatePublishClick = () => {
    setIsCreatePublicationOpen((prev) => !prev);
  };

  const handleCloseCreatePublishClick = () => {
    setIsCreatePublicationOpen((prev) => !prev);
  };

  return (
    <div className="sidebar">
      {isCreatePublicationOpen &&
        createPortal(
          <div className="create-publication-modal-overlay">
            <CreatePublication onCloseClick={handleCloseCreatePublishClick} />
          </div>,
          document.body
        )}
      <div className="wrapper">
        <div className="head">
          <img src="/android-chrome-512x512.png" alt="logo" />
          <span>Social Campus</span>
        </div>
        <div className="navigation">
          <ul>
            {sidebarItems.map(
              ({
                path,
                label,
                inactiveIcon: InactiveIcon,
                activeIcon: ActiveIcon,
              }) => (
                <li key={path}>
                  <NavItem
                    key={path}
                    path={path}
                    label={label}
                    inactiveIcon={InactiveIcon}
                    activeIcon={ActiveIcon}
                    hoveredIcon={hoveredIcon}
                    setHoveredIcon={setHoveredIcon}
                  />
                </li>
              )
            )}
          </ul>
          <div className="button-wrapper">
            <button
              className="add-publish"
              onClick={handleOpenCreatePublishClick}
            >
              Publish
            </button>
          </div>
        </div>
      </div>
      <div className="logout">
        <ShortProfile
          username={user.firstName + " " + user.lastName}
          login={user.login || ""}
          profileImage={user.profileImageUrl ? user.profileImageUrl : null}
        />
        <div
          onClick={handleLogout}
          className="logout-icon"
          onMouseEnter={() => setHoveredIcon("logout")}
          onMouseLeave={() => setHoveredIcon("")}
        >
          {hoveredIcon === "logout" ? (
            <IoExit className="exit-icon" />
          ) : (
            <IoExitOutline className="exit-icon" />
          )}
        </div>
      </div>
    </div>
  );
}

export default Sidebar;
