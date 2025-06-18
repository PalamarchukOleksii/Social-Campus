import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "./TagDetail.css";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import NavItem from "../../components/navItem/NavItem";
import PublicationDetailItems from "../../utils/consts/PublicationDetailItems";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import Loading from "../../components/loading/Loading";

const PUBLICATIONS_FOR_TAG_BASE_URL = "/api/publications/taglabel";
const PAGE_SIZE = 10;

function TagDetail() {
  const axios = useAxiosPrivate();
  const { tag } = useParams();
  const navigate = useNavigate();

  const [relatedPublications, setRelatedPublications] = useState([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [allFetched, setAllFetched] = useState(false);
  const [hoveredIcon, setHoveredIcon] = useState("");

  const fetchPublications = async () => {
    if (!tag || loading || allFetched) return;

    setLoading(true);
    try {
      const response = await axios.get(
        `${PUBLICATIONS_FOR_TAG_BASE_URL}/${tag}/count/${PAGE_SIZE}/page/${page}`
      );
      const newPublications = response.data;

      if (newPublications.length === 0) {
        setAllFetched(true);
        return;
      }

      setRelatedPublications((prev) => [...prev, ...newPublications]);
      setPage((prev) => prev + 1);
    } catch (error) {
      console.error("Error fetching publications:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setRelatedPublications([]);
    setPage(1);
    setAllFetched(false);
    fetchPublications();
  }, [tag, axios]);

  return (
    <div className="tag-detail-container">
      <div className="top-section">
        <NavItem
          label={PublicationDetailItems.label}
          inactiveIcon={PublicationDetailItems.inactiveIcon}
          activeIcon={PublicationDetailItems.activeIcon}
          hoveredIcon={hoveredIcon}
          setHoveredIcon={setHoveredIcon}
          onClick={() => navigate(-1)}
        />
        <h1 className="general-text">Publications for #{tag}</h1>
      </div>

      {loading && relatedPublications.length === 0 ? (
        <Loading />
      ) : relatedPublications.length > 0 ? (
        <>
          <PublicationsList publications={relatedPublications} />
          {!allFetched && (
            <div className="load-more-container">
              <button onClick={fetchPublications}>Load More</button>
            </div>
          )}
        </>
      ) : (
        <h2 className="not-found-users general-text">
          No publications found for this tag.
        </h2>
      )}
    </div>
  );
}

export default TagDetail;
