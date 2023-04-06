import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup } from 'react-bootstrap';

import { getMonthName } from '../../../utils';
import { getArchives } from '../../../services/widgets';

export default function ArchivesWidget() {
	// Component's states
	const [archives, setArchives] = useState([]);

	useEffect(() => {
		fetchArchives();

		async function fetchArchives() {
			const data = await getArchives(12);
			if (data) setArchives(data);
			else setArchives([]);
		}
	}, []);

	return (
		<div className='mb-4'>
			<h3 className='mb-2 text-success'>Bài viết theo tháng</h3>
			{archives.length > 0 && (
				<ListGroup>
					{archives.map((archive, index) => {
						return (
							<ListGroup.Item key={index}>
								<Link
									to={`/blog/archive/${archive.year}/${archive.month}`}
								>
									{`${getMonthName(archive.month)} ${
										archive.year
									} (${archive.postCount})`}
								</Link>
							</ListGroup.Item>
						);
					})}
				</ListGroup>
			)}
		</div>
	);
}
