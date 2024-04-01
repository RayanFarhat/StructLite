* note: $n$ is the number of nodes in the model multiplied by 6(the number of degrees of freedom).

## Model global stiffness matrix $K$
The model have global stiffness matrix of size nXn.

We get the global matrix from adding every stiffness matrix for each member element in the model.

## Reactions force vector $F_{er}$
The $F_{er}$ vector is calculating by iterating over each local $f_{er}$ for each element and adding them to get the $F_{er}$ vector.
Then we iterate 
* The $F_{er}$ vector size is $n$.

The $F_{er}$ vector is partitioned to $F_{er}1$ and  $F_{er}2$.
* $F_{er}1$ is the reactions of the degrees of freedom that are not supported(free and unknown).
* $F_{er}2$ is the reactions of the degrees of freedom that are supported(known) where displacment is zero.

## External force vector $F$
The $F$ vector is opposite of the reaction force vector $F_{er}1$.
Because when the DOF is not supported and there is no reaction, then the applied external force is equal to the other direction of the reaction.

* The $F$ vector size is $n$.

## Analysis
1- First we partition $K$ to $K_{11}$,$K_{12}$,$K_{21}$,$K_{22}$. based of the known and unknown displacment. just like in static condensation ($K_{11}$ is for the unknown).
2- The analysis allowed when the structure is stable(there is no rigid body motion).
This is checked by checking if $K_{11}$ is non-singular, non-singular mean that the structure is stable.
3- The global displacment vector $D$ is partitioned to $D1$ and $D2$, $D2$ is all zeros because it is known.
We calculate $D1$ as $D1 = K_{11}^{-1}F$.
4- Once we found $D1$ then we found $D$, then the analysis is done.

#### why when $K_{11}$ is singular then  the structure is unstable?
1- Definition of a Singular Matrix: A matrix is said to be singular if its determinant is zero. In the context of structural analysis, the stiffness matrix represents the relationship between applied forces and resulting displacements or deformations within the structure. If the stiffness matrix is singular, it means that there exists a set of displacements or deformations for which the applied forces cannot be uniquely determined, leading to infinite solutions.
2- Rigid Body Motion and Degrees of Freedom: Rigid body motion involves translational and rotational movements of the entire structure without any internal deformation. When the stiffness matrix is singular, it indicates the presence of one or more degrees of freedom associated with rigid body motion. These degrees of freedom do not contribute to resisting external loads, making the structure unstable.