import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup } from 'react-bootstrap';

import { getCategories } from '../services/categoriesWidget';

export default function CategoriesWidget() {
	// Component's states
	const [categories, setCategories] = useState([]);

	useEffect(() => {
		fetchCategories();

		async function fetchCategories() {
			const data = await getCategories();
			if (data) setCategories(data.items);
			else setCategories([]);
		}
	}, []);

	return (
		<div className='mb-4'>
			<h3 className='mb-2 text-success'>Các chủ đề</h3>
			{categories.length > 0 && (
				<ListGroup>
					{categories.map((category, index) => (
						<ListGroup.Item key={index}>
							<Link
								to={`/blog/category?slug=${category.urlSlug}`}
								title={category.description}
							>
								{category.name}
								<span>&nbsp;({category.postCount})</span>
							</Link>
						</ListGroup.Item>
					))}
				</ListGroup>
			)}
		</div>
	);
}
