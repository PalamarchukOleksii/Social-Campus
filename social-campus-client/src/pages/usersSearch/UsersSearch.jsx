import React, { useState, useEffect, useRef } from "react";
import SearchTabs from "../../components/searchTabs/SearchTabs";
import "./UsersSearch.css";
import ShortProfile from "../../components/shortProfile/ShortProfile";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import Loading from "../../components/loading/Loading";

const SEARCH_USERS_BASE_URL = "/api/users/searchterm";
const PAGE_SIZE = 10;

function UsersSearch() {
  const axios = useAxiosPrivate();

  const [searchQuery, setSearchQuery] = useState("");
  const [searchResultUsers, setSearchResultUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [allFetched, setAllFetched] = useState(false);
  const [page, setPage] = useState(1);

  const debounceTimeoutRef = useRef(null);

  const fetchUsers = async (reset = false, pageNum = 1) => {
    try {
      setLoading(true);

      const response = await axios.get(
        `${SEARCH_USERS_BASE_URL}/${searchQuery}/count/${PAGE_SIZE}/page/${pageNum}`
      );
      const users = response.data;

      if (reset) {
        setSearchResultUsers(users);
      } else {
        setSearchResultUsers((prev) => [...prev, ...users]);
      }

      setPage(pageNum + 1);

      if (users.length < PAGE_SIZE) {
        setAllFetched(true);
      } else {
        setAllFetched(false);
      }
    } catch (error) {
      console.error("Error fetching users:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (debounceTimeoutRef.current) clearTimeout(debounceTimeoutRef.current);

    if (searchQuery.trim().length === 0) {
      setSearchResultUsers([]);
      setAllFetched(false);
      setPage(1);
      setLoading(false);
      return;
    }

    debounceTimeoutRef.current = setTimeout(() => {
      fetchUsers(true, 1);
    }, 250);

    return () => {
      if (debounceTimeoutRef.current) clearTimeout(debounceTimeoutRef.current);
    };
  }, [searchQuery, axios]);

  return (
    <div className="search-page-container">
      <input
        type="text"
        placeholder="Type to search for users..."
        className="user-search-input"
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />
      <SearchTabs />
      <div className="users-list">
        {loading && searchResultUsers.length === 0 ? (
          <Loading />
        ) : searchResultUsers.length > 0 ? (
          searchResultUsers.map((user) => (
            <div key={user.id.value} className="search-result-user-container">
              <ShortProfile key={user.id.value} userId={user.id.value} />
              {user.bio && <h2 className="bio">{user.bio}</h2>}
            </div>
          ))
        ) : (
          <h2 className="not-found-users general-text">No users found.</h2>
        )}
      </div>
      {!allFetched && !loading && searchResultUsers.length > 0 && (
        <div className="load-more-container">
          <button onClick={() => fetchUsers(false, page)}>Load More</button>
        </div>
      )}
    </div>
  );
}

export default UsersSearch;
