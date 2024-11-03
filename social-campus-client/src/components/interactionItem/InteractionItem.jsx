import React, { useState } from "react";
import PropTypes from "prop-types";
import "./InteractionItem.css";

function InteractionItem(props) {
  const [isFavorited, setIsFavorited] = useState(false);
  const [likesCount, setLikesCount] = useState(props.label);

  const toggleFavorite = props.activeIcon
    ? () => {
        setIsFavorited((prev) => !prev);
        setLikesCount((prev) => (isFavorited ? prev - 1 : prev + 1));
      }
    : undefined;

  return (
    <div className="interaction-item" onClick={toggleFavorite}>
      {isFavorited ? (
        props.activeIcon ? (
          <props.activeIcon className="icon" />
        ) : (
          <props.icon className="icon" />
        )
      ) : (
        <props.icon className="icon" />
      )}
      <span className="stat">{likesCount}</span>
    </div>
  );
}

InteractionItem.propTypes = {
  label: PropTypes.number.isRequired,
  icon: PropTypes.elementType.isRequired,
  activeIcon: PropTypes.elementType,
};

export default InteractionItem;
