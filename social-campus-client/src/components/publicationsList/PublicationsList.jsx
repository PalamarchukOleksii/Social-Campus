import React from "react";
import PropTypes from "prop-types";
import Publication from "../publication/Publication";
import "./PublicationsList.css";

function PublicationsList({ publications }) {
  return (
    <div className="publications">
      {publications.map((publication) => (
        <div key={publication.id.value}>
          <Publication publication={publication} />
        </div>
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
      imageUrl: PropTypes.string.isRequired,
      creationDateTime: PropTypes.string.isRequired,
      creatorId: PropTypes.shape({
        value: PropTypes.string.isRequired,
      }).isRequired,
      userWhoLikedIds: PropTypes.arrayOf(
        PropTypes.shape({
          value: PropTypes.string.isRequired,
        })
      ),
      commentsCount: PropTypes.number.isRequired,
    })
  ).isRequired,
};

export default PublicationsList;
