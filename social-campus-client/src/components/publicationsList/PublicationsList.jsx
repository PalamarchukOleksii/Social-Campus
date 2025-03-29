import React from "react";
import PropTypes from "prop-types";
import Publication from "../publication/Publication";
import "./PublicationsList.css";

function PublicationsList(props) {
  return (
    <div className="publications">
      {props.publications.map((publication) => (
        <Publication key={publication.id.value} publication={publication} />
      ))}
    </div>
  );
}

PublicationsList.propTypes = {
  publications: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.shape({
        value: PropTypes.string.isRequired,
      }).isRequired,
      description: PropTypes.string.isRequired,
      imageData: PropTypes.string,
      creationDateTime: PropTypes.string.isRequired,
      creatorInfo: PropTypes.shape({
        id: PropTypes.shape({
          value: PropTypes.string.isRequired,
        }).isRequired,
        login: PropTypes.string.isRequired,
        firstName: PropTypes.string,
        lastName: PropTypes.string,
        bio: PropTypes.string,
        profileImageData: PropTypes.string,
      }).isRequired,
      userWhoLikedIds: PropTypes.arrayOf(PropTypes.string),
      commentsCount: PropTypes.number.isRequired,
    })
  ).isRequired,
};

export default PublicationsList;
