import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import { useCreateItem } from "../../context/CreateItemContext";
import { IoAdd } from "react-icons/io5";
import "./HorizontalNavbar.css";
import useAuth from "../../hooks/useAuth";
import GetSidebarItems from "../../utils/consts/GetSidebarItems";

function HorizontalNavbar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");

  const { openCreatePublication } = useCreateItem();
  const { auth } = useAuth();

  const handleLogout = async (e) => {
    e.preventDefault();
    try {
      navigate("/");
    } catch (error) {
      console.error(error);
    }
  };

  const handlePublishClick = () => {
    openCreatePublication();
  };

  const sidebarItems = GetSidebarItems(auth.shortUser.login);

  return (
    <div className="horizontal-navbar">
      <button className="add-publish-short" onClick={handlePublishClick}>
        <IoAdd />
      </button>
      {sidebarItems.map(
        ({
          path,
          label,
          inactiveIcon: InactiveIcon,
          activeIcon: ActiveIcon,
        }) => (
          <>
            <NavItem
              path={path}
              label={label}
              inactiveIcon={InactiveIcon}
              activeIcon={ActiveIcon}
              hoveredIcon={hoveredIcon}
              setHoveredIcon={setHoveredIcon}
              showLabel={false}
            />
          </>
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
