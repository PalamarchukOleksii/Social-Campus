import { IoArrowBackCircleOutline, IoArrowBackCircle } from "react-icons/io5";
import ROUTES from "./Routes";

const FollowTabsItems = [
  {
    path: `${ROUTES.PROFILE}`,
    label: "Profile",
    inactiveIcon: IoArrowBackCircleOutline,
    activeIcon: IoArrowBackCircle,
  },
  {
    path: `${ROUTES.FOLLOWERS}`,
    label: "Followers",
  },
  {
    path: `${ROUTES.FOLLOWING}`,
    label: "Following",
  },
];

export default FollowTabsItems;
