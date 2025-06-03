import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import Publication from "../publication/Publication";
import "./PublicationsList.css";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { toast } from "react-toastify";
import Loading from "../loading/Loading";

const USERS_BASE_URL = "/api/users";

function PublicationsList({ userId }) {
  const axios = useAxiosPrivate();
  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPublications = async () => {
      try {
        setLoading(true);
        const response = await axios.get(
          `${USERS_BASE_URL}/${userId.value}/publications`
        );
        setPublications(response.data);
      } catch (err) {
        toast.error(err.message || "Failed to fetch publications");
      } finally {
        setLoading(false);
      }
    };

    fetchPublications();
  }, [userId, axios]);

  if (loading) return <Loading />;

  return (
    <div className="publications">
      {publications.length > 0 ? (
        publications.map((publication) => (
          <Publication key={publication.id.value} publication={publication} />
        ))
      ) : (
        <h2 className="no-publications-text general-text">
          No publications yet
        </h2>
      )}
    </div>
  );
}

PublicationsList.propTypes = {
  userId: PropTypes.shape({
    value: PropTypes.string.isRequired,
  }).isRequired,
};

export default PublicationsList;
