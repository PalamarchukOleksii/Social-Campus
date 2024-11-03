import React from "react";
import "./NavItem.css";
import PropTypes from "prop-types";
import { NavLink } from "react-router-dom";

function NavItem(props) {
  return (
    <div className="nav-item">
      <li>
        <NavLink
          to={props.path}
          onMouseEnter={() => props.setHoveredIcon(props.label.toLowerCase())}
          onMouseLeave={() => props.setHoveredIcon("")}
          className={({ isActive }) => (isActive ? "link active" : "link")}
        >
          {({ isActive }) => (
            <>
              {props.hoveredIcon === props.label.toLowerCase() || isActive ? (
                <props.activeIcon className="icon" />
              ) : (
                <props.inactiveIcon className="icon" />
              )}
              <span>{props.label}</span>
            </>
          )}
        </NavLink>
      </li>
    </div>
  );
}

NavItem.propTypes = {
  activeIcon: PropTypes.elementType.isRequired,
  inactiveIcon: PropTypes.elementType.isRequired,
  label: PropTypes.string.isRequired,
  path: PropTypes.string.isRequired,
  setHoveredIcon: PropTypes.func.isRequired,
  hoveredIcon: PropTypes.string.isRequired,
};

export default NavItem;
