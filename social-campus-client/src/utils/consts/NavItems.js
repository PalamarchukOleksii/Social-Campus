import {
  IoHomeOutline,
  IoSearchOutline,
  IoChatboxEllipsesOutline,
  IoPersonOutline,
  IoHomeSharp,
  IoSearchSharp,
  IoChatboxEllipses,
  IoPersonSharp,
} from "react-icons/io5";

const NavItems = [
  {
    path: "/home",
    label: "Home",
    inactiveIcon: IoHomeOutline,
    activeIcon: IoHomeSharp,
  },
  {
    path: "/search",
    label: "Search",
    inactiveIcon: IoSearchOutline,
    activeIcon: IoSearchSharp,
  },
  {
    path: "/messages",
    label: "Messages",
    inactiveIcon: IoChatboxEllipsesOutline,
    activeIcon: IoChatboxEllipses,
  },
  {
    path: "/profile",
    label: "Profile",
    inactiveIcon: IoPersonOutline,
    activeIcon: IoPersonSharp,
  },
];

export default NavItems;
