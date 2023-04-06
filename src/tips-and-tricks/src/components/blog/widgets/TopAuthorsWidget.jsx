import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup } from 'react-bootstrap';

import { getTopAuthors } from '../../../services/widgets';

export default function TopAuthorsWidget() {
	// Component's states
	const [authors, setAuthors] = useState([]);

	useEffect(() => {
		fetchAuthors();

		async function fetchAuthors() {
			const data = await getTopAuthors(4);
			if (data) setAuthors(data);
			else setAuthors([]);
		}
	}, []);

	return (
		<div className='mb-4'>
			<h3 className='mb-2 text-success'>Tác giả nổi bật</h3>
			{authors.length > 0 && (
				<ListGroup>
					{authors.map((author, index) => {
						return (
							<ListGroup.Item key={index}>
								<Link to={`/blog/author/${author.urlSlug}`}>
									{author.fullName}
								</Link>
							</ListGroup.Item>
						);
					})}
				</ListGroup>
			)}
		</div>
	);
}
