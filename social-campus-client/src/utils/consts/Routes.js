const ROUTES = {
  LANDING: "/",
  HOME: "/home",
  SIGN_IN: "/signin",
  SIGN_UP: "/signup",
  USERS_SEARCH: "/search/users",
  TAGS_SEARCH: "/search/tags",
  PROFILE: "/profile/:login",
  MESSAGES: "/messages",
  FOLLOWERS: "/profile/:login/followers",
  FOLLOWING: "/profile/:login/following",
  PUBLICATIONDETAILS: "/publication/:id",
  EDIT_PROFILE: "/profile/:login/edit",
  TAGDETAILS: "/tag/:tag",
  PRIVATECHAT: "/messages/:login",
};

export default ROUTES;
