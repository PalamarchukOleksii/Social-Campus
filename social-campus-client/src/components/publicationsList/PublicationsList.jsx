import React from "react";
import PropTypes from "prop-types";
import Publication from "../publication/Publication";
import "./PublicationsList.css";

function PublicationsList(props) {
  return (
    <div className="publications">
      {props.publications.map((publication, index) => (
        <Publication key={index} publication={publication} />
      ))}
    </div>
  );
}

PublicationsList.propTypes = {
  publications: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      description: PropTypes.string,
      imageUrl: PropTypes.string,
      creationTime: PropTypes.string,
      likesCount: PropTypes.number,
      comments: PropTypes.arrayOf(PropTypes.object).isRequired,
      username: PropTypes.string,
      login: PropTypes.string,
      profileImage: PropTypes.string,
    })
  ).isRequired,
};

export default PublicationsList;
