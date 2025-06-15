import React from "react";
import PropTypes from "prop-types";
import Publication from "../publication/Publication";
import "./PublicationsList.css";

function PublicationsList({ publications, lastPublicationRef }) {
  return (
    <div className="publications">
      {publications.length > 0 ? (
        publications.map((publication, index) => {
          const isLast = index === publications.length - 1;

          return (
            <div
              key={publication.id.value}
              ref={isLast ? lastPublicationRef : null}
            >
              <Publication publication={publication} />
            </div>
          );
        })
      ) : (
        <h2 className="no-publications-text general-text">
          No publications yet
        </h2>
      )}
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
  lastPublicationRef: PropTypes.oneOfType([
    PropTypes.func,
    PropTypes.shape({ current: PropTypes.any }),
  ]),
};

export default PublicationsList;
