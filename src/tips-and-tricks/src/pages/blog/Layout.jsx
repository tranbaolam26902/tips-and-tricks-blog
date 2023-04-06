import { Outlet } from 'react-router-dom';

import styles from '../../styles/layout.module.css';

import Navigation from '../../components/Navigation';
import Sidebar from '../../components/Sidebar';

export default function Layout() {
	return (
		<>
			<Navigation />
			<div className={`container-fluid ${styles.content}`}>
				<div className='row'>
					<div className={`${styles.main} col-9`}>
						<Outlet />
					</div>
				</div>
				<div className={`${styles.sidebar} col-3 border-start`}>
					<Sidebar />
				</div>
			</div>
		</>
	);
}
