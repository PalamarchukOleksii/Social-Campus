import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import "./FollowItem.css";

function FollowItem(props) {
  const [isHovered, setIsHovered] = useState(false);
  const [buttonHoverType, setButtonHoverType] = useState("");
  const [buttonType, setButtonType] = useState("");

  const handleMouseEnter = () => {
    setIsHovered(true);
  };

  const handleMouseLeave = () => {
    setIsHovered(false);
  };

  const handleClick = () => {
    if (props.onClick) {
      props.onClick();
    }
  };

  useEffect(() => {
    if (isHovered) {
      if (props.hoveredText === "Unfollow") {
        setButtonHoverType("unfollow");
      } else {
        setButtonHoverType("follow");
      }
    } else {
      if (props.buttonText === "Follow") {
        setButtonType("followed");
        setButtonHoverType("");
      } else {
        setButtonType("following");
        setButtonHoverType("");
      }
    }
  }, [isHovered, props.hoveredText, props.buttonText]);

  return (
    <div className="item-container">
      <div className="top-part">
        <ShortProfile userId={props.userId} />
        <button
          className={`${buttonType} ${buttonHoverType}`}
          onMouseEnter={handleMouseEnter}
          onMouseLeave={handleMouseLeave}
          onClick={handleClick}
        >
          {isHovered ? props.hoveredText : props.buttonText}
        </button>
      </div>
      <div className="bottom-part">
        {props.bio && <h2 className="bio">{props.bio}</h2>}
      </div>
    </div>
  );
}

FollowItem.propTypes = {
  userId: PropTypes.string,
  bio: PropTypes.string,
  buttonText: PropTypes.string.isRequired,
  hoveredText: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
};

export default FollowItem;
