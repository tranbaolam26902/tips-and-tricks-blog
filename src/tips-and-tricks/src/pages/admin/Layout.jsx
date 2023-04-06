import { Outlet } from 'react-router-dom';

import styles from '../../styles/layout.module.css';

import Navigation from '../../components/admin/Navigation';

export default function Layout() {
	return (
		<>
			<Navigation />
			<div
				className='container-fluid'
				style={{ marginTop: 67 + 'px', marginBottom: 24 + 'px' }}
			>
				<Outlet />
			</div>
		</>
	);
}
