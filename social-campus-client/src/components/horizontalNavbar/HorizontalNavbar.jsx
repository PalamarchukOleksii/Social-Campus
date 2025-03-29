import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import { IoAdd } from "react-icons/io5";
import "./HorizontalNavbar.css";
import useAuth from "../../hooks/useAuth";
import GetSidebarItems from "../../utils/consts/GetSidebarItems";
import useLogout from "../../hooks/useLogout";
import ROUTES from "../../utils/consts/Routes";
import CreatePublication from "../createPublication/CreatePublication";

function HorizontalNavbar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");
  const [sidebarItems, setSidebarItems] = useState([]);
  const [isCreatePublicationOpen, setIsCreatePublicationOpen] = useState(false);

  const { auth } = useAuth();
  const logout = useLogout();

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
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
    setIsCreatePublicationOpen(true);
  };

  const handleCloseCreatePublishClick = () => {
    setIsCreatePublicationOpen(false);
  };

  return (
    <div className="horizontal-navbar">
      {isCreatePublicationOpen && (
        <div className="create-publication-modal-overlay">
          <CreatePublication onCloseClick={handleCloseCreatePublishClick} />
        </div>
      )}
      <button
        className="add-publish-short"
        onClick={handleOpenCreatePublishClick}
      >
        <IoAdd />
      </button>
      {sidebarItems.map(
        ({
          path,
          label,
          inactiveIcon: InactiveIcon,
          activeIcon: ActiveIcon,
        }) => (
          <NavItem
            key={path}
            path={path}
            label={label}
            inactiveIcon={InactiveIcon}
            activeIcon={ActiveIcon}
            hoveredIcon={hoveredIcon}
            setHoveredIcon={setHoveredIcon}
            showLabel={false}
          />
        )
      )}
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
  );
}

export default HorizontalNavbar;
