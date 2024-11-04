import React from "react";
import "./NavItem.css";
import PropTypes from "prop-types";
import { NavLink } from "react-router-dom";

function NavItem(props) {
  return (
    <div className="nav-item">
      <NavLink
        to={props.path}
        end
        onMouseEnter={() => props.setHoveredIcon(props.label.toLowerCase())}
        onMouseLeave={() => props.setHoveredIcon("")}
        className={({ isActive }) => (isActive ? "link active" : "link")}
      >
        {({ isActive }) => (
          <>
            {props.hoveredIcon === props.label.toLowerCase() || isActive ? (
              props.activeIcon ? (
                <props.activeIcon className="icon" />
              ) : null
            ) : props.inactiveIcon ? (
              <props.inactiveIcon className="icon" />
            ) : null}
            {props.label && <span>{props.label}</span>}
          </>
        )}
      </NavLink>
    </div>
  );
}

NavItem.propTypes = {
  activeIcon: PropTypes.elementType,
  inactiveIcon: PropTypes.elementType,
  label: PropTypes.string,
  path: PropTypes.string.isRequired,
  setHoveredIcon: PropTypes.func.isRequired,
  hoveredIcon: PropTypes.string.isRequired,
};

export default NavItem;
