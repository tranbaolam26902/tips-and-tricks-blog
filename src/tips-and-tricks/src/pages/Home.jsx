import { useEffect } from 'react';

export default function Home() {
	useEffect(() => {
		document.title = 'Trang chủ';
	}, []);

	return <h1>Trang chủ</h1>;
}
